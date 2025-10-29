namespace Application.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se intenta crear una entidad que ya existe en el sistema
    /// </summary>
    public class EntityDoesExistException : Exception
    {
        public EntityDoesExistException()
            : base("La entidad ya existe en el sistema")
        {
        }

        public EntityDoesExistException(string message)
            : base(message)
        {
        }
    }
}
