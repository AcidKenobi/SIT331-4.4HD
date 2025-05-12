public class MapOld {
    public int Id {get;set;}
    public int Columns {get;set;}
    public int Rows{get;set;}
    public string Name {get;set;} //Name of the map, e.g. Left, right, place??
    public string? Description {get;set;}
    public DateTime CreatedDate {get;set;}
    public DateTime ModifiedDate {get;set;}
    public bool IsSquare{ get; set; }

    public MapOld(int Id, int Columns, int Rows, string Name, string? Description, DateTime CreatedDate, DateTime ModifiedDate)
    {
        this.Id = Id;
        this.Columns = Columns;
        this.Rows = Rows;
        this.Name = Name;
        this.Description = Description;
        this.CreatedDate = CreatedDate;
        this.ModifiedDate = ModifiedDate;
    }

    public MapOld(){}
}