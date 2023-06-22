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

# Build

```
$ git clone https://github.com/usptact/Infer.NET-Pyramid.git
$ cd Infer.NET-Pyramid
$ dotnet build -c Debug Pyramid.sln
```

# Run

```
$ dotnet run --project Pyramid
```

# Credits
The problem is taken from [UKMT Junior Maths Challenge 2022](https://www.youtube.com/watch?v=K_BCGD-ijOY)

