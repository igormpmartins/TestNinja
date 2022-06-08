namespace TestNinja.Mocking
{
    public interface IEmployeeStorage
    {
        void DeleteEmployee(int id);
    }

    public class EmployeeStorage : IEmployeeStorage
    {
        private readonly EmployeeContext context;

        public EmployeeStorage(EmployeeContext context)
        {
            this.context = context;
        }

        public void DeleteEmployee(int id)
        {
            var employee = context.Employees.Find(id);
            if (employee == null) return;
            
            context.Employees.Remove(employee);
            context.SaveChanges();
        }

    }
}