namespace AutoMapper.MultiTargetMapping.Tests.Models
{

    public class AModel
    {

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public override string ToString()
        {
            return $"A: Field1 = {this.Field1}, Field2 = {this.Field2}";
        }

    }

}
