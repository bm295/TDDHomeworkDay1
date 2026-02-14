# TDDHomeworkDay1
[![Build Status](https://travis-ci.org/bm295/TDDHomeworkDay1.svg?branch=master)](https://travis-ci.org/github/bm295/TDDHomeworkDay1)

## .NET 10 Setup
This repository now targets **.NET 10** (`net10.0`).

## Run the async snippet
The snippet lives in `Shop/Program.cs` and can be run from the repository root:

```bash
dotnet run --project Shop/Shop.csproj
```

Expected behavior: it starts 3 async tasks, waits for all of them to complete, and prints values to the console.
