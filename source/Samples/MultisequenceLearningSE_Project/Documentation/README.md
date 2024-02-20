# ML23/24-09   Approve Prediction of Multisequence Learning 

## Introduction

In this project, we attempted to build new ways using the 'MultisequenceLearning' algorithm. The new methods automatically read the dataset from the supplied path in 'HelperMethods.ReadDataset(datasetPath)'. We also have test data in another file that has to be read for later testing the subsequences in a similar format to 'HelperMethods.ReadDataset(testsetPath)'. 'RunMultiSequenceLearningExperiment(sequences, sequencesTest)' accepts the numerous sequences in'sequences' and the test subsequences in'sequencesTest' and passes them to 'RunMultiSequenceLearningExperiment(sequences, sequencesTest)'. After the learning process is done, the predicted element's accuracy is calculated. 
## Implementation

![image](./images/overview.png)

Fig: Design of Approve Prediction of Multisequence Learning

Above our project's implementation flow.

The model used to process and store the dataset is called `Sequence`. And as shown below:

```csharp
public class Sequence
{
    public String name { get; set; }
    public int[] data { get; set; }
}
```

eg:
- Dataset

```json
[
  {
    "name": "S1",
    "data": [ 0, 2, 5, 6, 7, 8, 10, 11, 13 ]
  },
  {
    "name": "S2",
    "data": [ 1, 2, 3, 4, 6, 11, 12, 13, 14 ]
  },
  {
    "name": "S3",
    "data": [ 1, 2, 3, 4, 7, 8, 10, 12, 14 ]
  }
]
```

- Test Dataset

```json
[
  {
    "name": "T1",
    "data": [ 1, 2, 4 ]
  },
  {
    "name": "T2",
    "data": [ 2, 3, 4 ]
  },
  {
    "name": "T3",
    "data": [ 4, 5, 7 ]
  },
  {
    "name": "T4",
    "data": [ 5, 8, 9 ]
  }
]

```
