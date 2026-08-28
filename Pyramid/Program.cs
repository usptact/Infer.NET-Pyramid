using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Compiler;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Models;

// With <ImplicitUsings> the global `using System;` pulls in System.Range, which
// collides with Microsoft.ML.Probabilistic.Models.Range. Alias the Infer.NET one
// so `InferRange` is always unambiguous and a bare `Range` still means System.Range.
using InferRange = Microsoft.ML.Probabilistic.Models.Range;

/*
 * The pyramid is given:
 *
 *          +----+
 *          | 61 |
 *        +----+----+
 *        |    |    |
 *     +----+----+----+
 *     |    | 16 |    |
 *   +----+----+----+----+
 *   |    |    |    |    |
 * +----+----+----+----+----+
 * |    |    |  ? |    |    |
 * +----+----+----+----+----+
 *
 * Constraints:
 *   (1) The value of a cell is the sum of the two cells directly below it.
 *   (2) The sum of the 5 base-layer cells is 17.
 *
 * Task: infer the value of the cell marked with "?" (the exact answer is 4).
 *
 * Credits: https://www.youtube.com/watch?v=K_BCGD-ijOY
 */

const double unknownVariance = 100.0; // base-layer cells: essentially no prior knowledge
const double knownVariance = 1.0;     // the sum rule and observations get a little slack

// --- Base layer: 5 unknown cells, modelled as an Infer.NET random-variable array ---
InferRange baseCells = new(5);
VariableArray<double> baseLayer = Variable.Array<double>(baseCells).Named("baseLayer");
baseLayer[baseCells] = Variable.GaussianFromMeanAndVariance(0.0, unknownVariance).ForEach(baseCells);
Variable.ConstrainPositive(baseLayer[baseCells]);

// Constraint (2): the five base cells sum to 17.
Variable.ConstrainEqual(Variable.Sum(baseLayer), 17.0);

// --- Build the pyramid bottom-up ---
// rows[0] is the 5-cell base; each row above is one cell shorter, and every cell
// is a noisy sum of the two cells directly beneath it (constraint 1).
var rows = new List<Variable<double>[]>
{
    new Variable<double>[] { baseLayer[0], baseLayer[1], baseLayer[2], baseLayer[3], baseLayer[4] },
};

for (int row = 1; row <= 4; row++)
{
    Variable<double>[] below = rows[row - 1];
    var current = new Variable<double>[below.Length - 1];
    for (int i = 0; i < current.Length; i++)
    {
        current[i] = Variable.GaussianFromMeanAndVariance(below[i] + below[i + 1], knownVariance)
                             .Named($"l{row}_{i}");
        Variable.ConstrainPositive(current[i]);
    }
    rows.Add(current);
}

// --- Observations ---
rows[4][0].ObservedValue = 61.0; // apex
rows[2][1].ObservedValue = 16.0; // middle cell of the third row from the top

// --- Inference ---
var engine = new InferenceEngine(new ExpectationPropagation())
{
    NumberOfIterations = 100,
};
// Force the Roslyn compiler; the legacy CodeDOM backend is unsupported on some platforms.
engine.Compiler.CompilerChoice = CompilerChoice.Roslyn;

Gaussian[] basePosterior = engine.Infer<Gaussian[]>(baseLayer);
Console.WriteLine($"Dist over ? (base-layer middle cell) = {basePosterior[2]}");
