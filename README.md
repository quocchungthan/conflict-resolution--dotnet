# Run the project
Just update the Data/ folder then:
```
dotnet run
```
> Check the `global.json` for required dotnet version.

## Flow And approach:
```mermaid
flowchart TD

    A[Start] --> B[Load all test data from Data/ folder]
    B --> C[For each test case]
    C --> D[Compare Original vs Persisted → persistedChanges]
    C --> E[Compare Original vs New → requestedChanges]
    D --> F[Resolve Conflicts]

    %% Conflict resolution process
    subgraph G[ConflictResolutionService.Resolve()]
        F --> G1[Iterate persisted & requested lines]
        G1 --> G2{Compare line types}

        G2 -->|Both Unchanged| G3[Keep original line]
        G2 -->|Only Requested changed| G4[Accept requested change]
        G2 -->|Only Persisted changed| G5[Accept persisted change]
        G2 -->|Both changed differently| G6[Throw TextConflictException]
        G2 -->|Both deleted same line| G7[Skip deletion]

        G3 --> G8[Continue]
        G4 --> G8
        G5 --> G8
        G6 --> G8
        G7 --> G8
    end

    G8 --> H[Append remaining non-deleted lines]
    H --> I[Merge all into final resolved HTML text]
    I --> J[Print result or conflict]
    J --> K[Next test case]
    K --> L[End]

    %% Styling for clarity
    style A fill:#aae,stroke:#336,stroke-width:1px,color:#fff
    style G fill:#eef,stroke:#446,stroke-width:1px
    style G6 fill:#faa,stroke:#933,stroke-width:1px


```
| Component                     | Responsibility                                                                                                                          |
| ----------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| **Program.cs**                | Orchestrates data loading, diffing, and conflict resolution.                                                                            |
| **DataLoaderService**         | Loads test cases (original, persisted, new HTML).                                                                                       |
| **TextCompareService**        | Uses `DiffPlex` to split HTML into logical “lines” and compare changes.                                                                 |
| **ConflictResolutionService** | Applies merge logic to combine two change sets safely; throws `TextConflictException` when both sides change the same line differently. |


## Tests again test data in Data/ folder
[!test result](./docs/testresult-27-10-2025.png)