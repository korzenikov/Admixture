module AdmixtureLib.Clustering


open Microsoft.ML
open Microsoft.ML.Trainers
open Microsoft.ML.Data
open Microsoft.ML.Transforms
open Microsoft.ML.Runtime.Api

    
type IrisData() = 
    [<Column("0"); DefaultValue>] 
    val mutable Label: single
    [<Column("1"); DefaultValue>] 
    val mutable SepalLength: single
    [<Column("2"); DefaultValue>] 
    val mutable SepalWidth: single
    [<Column("3"); DefaultValue>] 
    val mutable PetalLength:single
    [<Column("4"); DefaultValue>] 
    val mutable PetalWidth: single

type IrisDataTest() = 
    [<Column("0"); DefaultValue>] 
    val mutable Label: single
    [<ColumnName("Features"); VectorType(4); DefaultValue>] 
    val mutable Features: single[]

type IrisPrediction() =  
    [<ColumnName("Score"); DefaultValue>] 
    val mutable PredictedLabels: single[]

type ClusterPrediction() =
    [<ColumnName("PredictedLabel"); DefaultValue>]
    val mutable SelectedClusterId: uint32 

    [<ColumnName("Score"); DefaultValue>]
    val mutable Distance : single[]

let train =
    let pipeline = LearningPipeline()
        
    //let data = 
    //    [
    //        IrisDataTest(SepalLength = 1.0f, SepalWidth = 1.0f, PetalLength=0.3f, PetalWidth=5.1f, Label = 1.0f)
    //        IrisDataTest(SepalLength = 1.0f, SepalWidth = 1.0f, PetalLength=0.3f, PetalWidth=5.1f, Label =1.0f)
    //        IrisDataTest(SepalLength = 1.2f, SepalWidth = 0.5f, PetalLength=0.3f, PetalWidth=5.1f, Label = 0.0f)
    //    ]

    let data = 
        [
            IrisDataTest(Features = [| 1.0f; 1.0f; 0.3f; 5.1f |], Label = 1.0f)
            IrisDataTest(Features = [| 1.0f; 1.0f; 0.3f; 5.1f |], Label =1.0f)
            IrisDataTest(Features = [| 1.2f; 0.5f; 0.3f; 5.1f |], Label = 0.0f)
        ]


    let collection = CollectionDataSource.Create(data)
    pipeline.Add(collection)
    let clusterer = KMeansPlusPlusClusterer()
    clusterer.K <- 2
    pipeline.Add(clusterer)
    let model = pipeline.Train<IrisDataTest, ClusterPrediction>()
    let prediction = model.Predict(IrisDataTest(Features = [| 3.3f; 1.6f; 0.2f; 5.1f |]))
    //let prediction = model.Predict(IrisDataTest(SepalLength = 3.3f, SepalWidth = 1.6f,PetalLength = 0.2f,PetalWidth = 5.1f))
    0

