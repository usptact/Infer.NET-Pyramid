## Pyramid (Infer.NET on .NET 10)

**UKMT Junior Maths Problem (JMC)** modeled probabilistically with Infer.NET and solved via Expectation Propagation (EP).

### Problem statement
```
The following pyramid is given:

         +----+
         | 61 |
       +----+----+
       |    |    |
    +----+----+----+
    |    | 16 |    |
  +----+----+----+----+
  |    |    |    |    |
+----+----+----+----+----+
|    |    |  ? |    |    |
+----+----+----+----+----+

Constraints:

(1) The value of a cell is the sum of the two cells immediately below it.

(2) The sum of the 5 base layer cells is 17.

Task: Infer the value of the cell marked with "?"
```

The exact answer is `4`. This project instead *infers* it with
[Infer.NET](https://dotnet.github.io/infer/) by modelling every cell as a random
variable and running Expectation Propagation.

### How this repo solves it
- **Model**: Each cell is a Gaussian random variable. The base layer is an Infer.NET
  `VariableArray` over a `Range`; every cell above it is a noisy sum
  (`GaussianFromMeanAndVariance`) of the two cells directly beneath it. Positivity
  constraints keep values non-negative, and the base cells are constrained to sum to 17.
- **Inference**: Infer.NET's `ExpectationPropagation` algorithm infers the posterior
  over the middle base cell (the "?") given the observations (top cell 61, middle of
  the third row 16).

## Prerequisites
- **.NET SDK 10.0+** (verify with `dotnet --version`)
- Internet access for NuGet restore (to fetch `Microsoft.ML.Probabilistic` packages)

## Get the code
```
git clone https://github.com/usptact/Infer.NET-Pyramid.git
cd Infer.NET-Pyramid
```

## Build
```
dotnet restore Pyramid.slnx
dotnet build -c Release Pyramid.slnx
```

## Run
```
dotnet run -c Release --project Pyramid
```

You should see a line like:
```
Dist over ? (base-layer middle cell) = Gaussian(4.32, 4.398)
```

## Notes on versions
- Target framework: `net10.0`
- Infer.NET packages: `Microsoft.ML.Probabilistic` and `Microsoft.ML.Probabilistic.Compiler` (0.4.2504.701)
- `System.Range` (pulled in by `ImplicitUsings`) collides with `Microsoft.ML.Probabilistic.Models.Range`;
  the code aliases the Infer.NET type as `InferRange` to keep both usable.

## License
This project is licensed under the MIT License. See `LICENSE` for details.

## Credits
The problem is taken from the UKMT Junior Maths Challenge video explanation: https://www.youtube.com/watch?v=K_BCGD-ijOY
