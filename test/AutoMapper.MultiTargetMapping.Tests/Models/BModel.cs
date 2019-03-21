namespace AutoMapper.MultiTargetMapping.Tests.Models
{

    public class BModel
    {

        public string Field3 { get; set; }

        public string Field4 { get; set; }

        public override string ToString()
        {
            return $"B: Field1 = {this.Field3}, Field2 = {this.Field4}";
        }

    }

}
