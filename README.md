# Pyramid

JMC UKMT 2020

Junior Maths Problem

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

# Running

```
dotnet run --project Pyramid -c Release
```

Requires the .NET 10 SDK. Uses Infer.NET (`Microsoft.ML.Probabilistic`) 0.4.2504.701.

# Credits
The problem is taken from [UKMT Junior Maths Challenge 2022](https://www.youtube.com/watch?v=K_BCGD-ijOY)
