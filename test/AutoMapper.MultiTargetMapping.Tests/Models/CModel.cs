namespace AutoMapper.MultiTargetMapping.Tests.Models
{

    public class CModel
    {

        public string Field5 { get; set; }

        public string Field6 { get; set; }

        public override string ToString()
        {
            return $"C: Field1 = {this.Field5}, Field2 = {this.Field6}";
        }

    }

}
