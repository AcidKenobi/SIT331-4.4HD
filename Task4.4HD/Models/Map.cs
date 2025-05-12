using System;
using System.Collections.Generic;

namespace task3._2.Models {

    public class Map{

        public int Id { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool? IsSquare { get; set; }
        
        public Map(){
    
        }

        public Map(int Id, int Columns, int Rows, string Name, string? Description, DateTime CreatedDate, DateTime ModifiedDate, bool? IsSquare){
            this.Id = Id;
            this.Columns = Columns;
            this.Rows = Rows;
            this.Name = Name;
            this.Description = Description;
            this.CreatedDate = CreatedDate;
            this.ModifiedDate = ModifiedDate;
            this.IsSquare = IsSquare;
        }
    }
}