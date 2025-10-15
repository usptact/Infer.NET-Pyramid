## Pyramid (Infer.NET on .NET 8)

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

### How this repo solves it
- **Model**: Each cell is a Gaussian random variable. Summation constraints are encoded with Gaussian factors and explicit equality constraints. Positivity constraints enforce non-negative values.
- **Inference**: We use Infer.NET's `ExpectationPropagation` algorithm to infer the posterior over the middle base cell (the "?" value) given the observations (top cell 61 and the middle of the third row 16) and constraints.

## Prerequisites
- **.NET SDK 8.0+** (verify with `dotnet --version`)
- Internet access for NuGet restore (to fetch `Microsoft.ML.Probabilistic` packages)

## Get the code
```
git clone https://github.com/usptact/Infer.NET-Pyramid.git
cd Infer.NET-Pyramid
```

## Build
```
dotnet restore Pyramid.sln
dotnet build -c Release Pyramid.sln
```

## Run
```
dotnet run -c Release --project Pyramid
```

You should see a line like:
```
Dist over l0_3=Gaussian(Mean=..., Variance=...)
```

## Notes on versions
- Target framework: `net8.0`
- Infer.NET packages: `Microsoft.ML.Probabilistic` and `Microsoft.ML.Probabilistic.Compiler` (0.4.2301.301). If a newer version is available on NuGet, you can upgrade via:
```
dotnet add Pyramid package Microsoft.ML.Probabilistic
dotnet add Pyramid package Microsoft.ML.Probabilistic.Compiler
```

## License
This project is licensed under the MIT License. See `LICENSE` for details.

## Credits
The problem is taken from the UKMT Junior Maths Challenge video explanation: https://www.youtube.com/watch?v=K_BCGD-ijOY

