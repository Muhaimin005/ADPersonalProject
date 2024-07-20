namespace ADTest.Models.ViewModel
{
    public class MyStudentViewModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int Semester { get; set; }
        public string academicSession {  get; set; } 
        public string SupervisorName { get; set; }
        public int? ProposalId { get; set; } // Add ProposalId for navigation
        public string ProposalStatus { get; set; }
    }
}
