namespace Abp.Application.Services.Dto
{
    /// <summary>
    ///     This <see cref="IInputDto" /> can be directly used (or inherited)
    ///     to pass an Id value to an application service method.
    /// </summary>
    /// <typeparam name="TId">Type of the Id</typeparam>
    public class IdInput<TId> : IInputDto
    {
        public IdInput()
        {
        }

        public IdInput(TId id)
        {
            Id = id;
        }

        public TId Id { get; set; }
    }

    /// <summary>
    ///     A shortcut of <see cref="IdInput{TPrimaryKey}" /> for <see cref="int" />.
    /// </summary>
    public class IdInput : IdInput<int>
    {
        public IdInput()
        {
        }

        public IdInput(int id)
            : base(id)
        {
        }
    }
}