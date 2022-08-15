using BaseX;
using Newtonsoft.Json.Linq;

namespace FrooxEngine.LogiX.Json
{
    [NodeName("Add Object")]
    [NodeOverload("JSONAddObject")]
    [Category("LogiX/Json")]
    public class JSONAddDummy : LogixOperator<JObject>
    {
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        public readonly Input<dummy> Object;
        public override JObject Content => null;
    }
    [HiddenNode]
    [NodeName("Add String")]
    [NodeOverload("JSONAddObject")]
    [Category("LogiX/Json")]
    public class JSONAddString : LogixOperator<JObject>
    {
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        public readonly Input<string> Object;
        public override JObject Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                if (input == null) return null; //no jobject to add to, return nothing
                var tag = Tag.EvaluateRaw();
                var obj = Object.EvaluateRaw();
                //valid jobject, but invalid tag or object, return the jobject with no changes
                if (string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(obj)) return input;
                //jobjects are classes, not structs, so if this was stored somewhere or accessed in another context
                //i believe it would edit it for all instances, can someone verify if this is true?
                var in2 = (JObject)input.DeepClone();
                in2[tag] = obj;
                return in2;
            }
        }
    }
    [HiddenNode]
    [NodeName("Add Bool")]
    [NodeOverload("JSONAddObject")]
    [Category("LogiX/Json")]
    public class JSONAddBool : LogixOperator<JObject>
    {
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        public readonly Input<bool> Object;
        public override JObject Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                if (input == null) return null;
                var tag = Tag.EvaluateRaw();
                if (string.IsNullOrEmpty(tag)) return input;
                var in2 = (JObject)input.DeepClone();
                in2[tag] = Object.EvaluateRaw();
                return in2;
            }
        }
    }
    [HiddenNode]
    [NodeName("Add Nested Object")]
    [NodeOverload("JSONAddObject")]
    [Category("LogiX/Json")]
    public class JSONAddNestedObject : LogixOperator<JObject>
    {
        public readonly Input<JObject> Input;
        public readonly Input<string> Tag;
        public readonly Input<JObject> Object;
        public override JObject Content
        {
            get
            {
                var input = Input.EvaluateRaw();
                if (input == null) return null;
                var tag = Tag.EvaluateRaw();
                var obj = Object.EvaluateRaw();
                if (string.IsNullOrEmpty(tag) || obj == null) return input;
                var in2 = (JObject)input.DeepClone();
                in2[tag] = (JObject)obj.DeepClone();
                return in2;
            }
        }
    }
}