using System;
using System.Collections.Generic;

namespace task3._2.Models {

    public class RobotCommand{

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsMoveCommand { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        
        public RobotCommand(){
    
        }

        public RobotCommand(int Id, string Name, string? Description, bool IsMoveCommand, DateTime CreatedDate, DateTime ModifiedDate){
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.IsMoveCommand = IsMoveCommand;
            this.CreatedDate = CreatedDate;
            this.ModifiedDate = ModifiedDate;
        }
    }
}