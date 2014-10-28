using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonOperation
{
    public enum FeatureType
    {
        UNKOWN,
        FACTROW,
        RICHCARD,
        MAINLINEANSWER,
        CONTEXTREGIONANSWER
    }

    public class FeatureOutput
    {
        FeatureType _feature;
        string _output;

        public FeatureOutput(FeatureType feature, string output)
        {
            this._feature = feature;
            this._output = output;
        }

        public FeatureOutput()
        {
            this._feature = FeatureType.UNKOWN;
            this._output = "";
        }

        public FeatureType Feature
        {
            get
            {
                return _feature;
            }

            set
            {
                _feature = value;
            }
        }

        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
            }
        }
    }

    public class FeatureOutputUtility
    {
        public static FeatureOutput DeserializeFromJsonForOneFeatureOutput(string json)
        {
            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            FeatureOutput featureOutput = new FeatureOutput();
            featureOutput.Feature = (FeatureType)Enum.Parse(typeof(FeatureType), dic["Type"]);
            featureOutput.Output = dic["Data"];
            return featureOutput;
        }

        public static string SerializeToJsonForOneFeatureOutput(FeatureOutput featureOutput)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["Type"] = featureOutput.Feature.ToString();
            dic["Data"] = featureOutput.Output;
            return JsonConvert.SerializeObject(dic);
        }

    }

    class JsonConvertUsage
    {
    }
}
