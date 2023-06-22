using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Models;

namespace Pyramid {
    class Program {
        static void Main(string[] args) {
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
             * 
             * (1) The value of a cell is the sum of the two cells from below.
             * 
             * (2) The sum of the 5 base layer cells is 17.
             * 
             * Task: Infer the value of the cell marked with "?"
             * 
             * Credits: https://www.youtube.com/watch?v=K_BCGD-ijOY
             * 
             */

            // cells

            double unk_variance = 100.0;   // we know nothing about base layer values, let it be large if needed
            double know_variance = 1.0;    // we know the value of this cell but let some room for variance

            var l0_1 = Variable.GaussianFromMeanAndVariance(0.0, unk_variance).Named("l4_1");
            var l0_2 = Variable.GaussianFromMeanAndVariance(0.0, unk_variance).Named("l4_2");
            var l0_3 = Variable.GaussianFromMeanAndVariance(0.0, unk_variance).Named("l4_3");
            var l0_4 = Variable.GaussianFromMeanAndVariance(0.0, unk_variance).Named("l4_4");
            var l0_5 = Variable.GaussianFromMeanAndVariance(0.0, unk_variance).Named("l4_5");

            var l1_1 = Variable.GaussianFromMeanAndVariance(l0_1 + l0_2, know_variance).Named("l3_1 = l0_1 + l0_2");
            var l1_2 = Variable.GaussianFromMeanAndVariance(l0_2 + l0_3, know_variance).Named("l3_2 = l0_2 + l0_3");
            var l1_3 = Variable.GaussianFromMeanAndVariance(l0_3 + l0_4, know_variance).Named("l3_3 = l0_3 + l0_4");
            var l1_4 = Variable.GaussianFromMeanAndVariance(l0_4 + l0_5, know_variance).Named("l3_4 = l0_4 + l0_5");

            var l2_1 = Variable.GaussianFromMeanAndVariance(l1_1 + l1_2, know_variance).Named("l2_1 = l1_1 + l1_2");
            var l2_2 = Variable.GaussianFromMeanAndVariance(l1_2 + l1_3, know_variance).Named("l2_2 = l1_2 + l1_3");
            var l2_3 = Variable.GaussianFromMeanAndVariance(l1_3 + l1_4, know_variance).Named("l2_3 = l1_3 + l1_4");

            var l3_1 = Variable.GaussianFromMeanAndVariance(l2_1 + l2_2, know_variance).Named("l3_1 = l2_1 + l2_2");
            var l3_2 = Variable.GaussianFromMeanAndVariance(l2_2 + l2_3, know_variance).Named("l3_2 = l2_2 + l2_3");

            var l4 = Variable.GaussianFromMeanAndVariance(l3_1 + l3_2, know_variance).Named("l4 = l3_1 + l3_2");
            

            // positivity constraints (?)
            
            Variable.ConstrainPositive(l4);

            Variable.ConstrainPositive(l3_1);
            Variable.ConstrainPositive(l3_2);

            Variable.ConstrainPositive(l2_1);
            Variable.ConstrainPositive(l2_2);
            Variable.ConstrainPositive(l2_3);

            Variable.ConstrainPositive(l1_1);
            Variable.ConstrainPositive(l1_2);
            Variable.ConstrainPositive(l1_3);
            Variable.ConstrainPositive(l1_4);

            Variable.ConstrainPositive(l0_1);
            Variable.ConstrainPositive(l0_2);
            Variable.ConstrainPositive(l0_3);
            Variable.ConstrainPositive(l0_4);
            Variable.ConstrainPositive(l0_5);
            

            // equality constraints

            Variable.ConstrainEqual(l4, l3_1 + l3_2);

            Variable.ConstrainEqual(l3_1, l2_1 + l2_2);
            Variable.ConstrainEqual(l3_2, l2_2 + l2_3);

            Variable.ConstrainEqual(l2_1, l1_1 + l1_2);
            Variable.ConstrainEqual(l2_2, l1_2 + l1_3);
            Variable.ConstrainEqual(l2_3, l1_3 + l1_4);

            Variable.ConstrainEqual(17.0, l0_1 + l0_2 + l0_3 + l0_4 + l0_5);


            // observed values

            l4.ObservedValue = 61.0;

            l2_2.ObservedValue = 16.0;

            // infer l0_3

            InferenceEngine engine = new InferenceEngine();
            engine.NumberOfIterations = 100;
            engine.Algorithm = new ExpectationPropagation();

            Console.WriteLine("Dist over l0_3=" + engine.Infer(l0_3));
            Console.WriteLine("\n*** Press any key to exit ***");
            Console.ReadKey();
        }
    }
}
