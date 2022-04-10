using System.Runtime.Serialization;

namespace Core.Models.Dashboard
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint (string label, double y)
        {
            Label = label;
            Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}
