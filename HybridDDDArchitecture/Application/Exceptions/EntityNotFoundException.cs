namespace Application.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra una entidad solicitada
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }
}