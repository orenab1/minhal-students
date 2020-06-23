using System.Collections.Generic;

namespace GovILApi.ViewModels
{
    public class PersonIdObj
    {
        public string idNum { get; set; }
        public int[] personRelationship { get; set; }
        public PersonInfoObj[] personInfo { get; set; }
    }
}