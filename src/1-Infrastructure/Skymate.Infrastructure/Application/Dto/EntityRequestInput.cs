namespace Skymate.Application.Dto
{
    using System;

    using Skymate.Application;

    /// <summary>
    /// This <see cref="IInputDto"/> can be used to send Id of an entity to an <see cref="IApplicationService"/> method.
    /// </summary>
    [Serializable]
    public class EntityRequestInput : EntityRequestInput<Guid>, IEntityDto
    {
        /// <summary>
        /// Creates a new <see cref="EntityRequestInput"/> object.
        /// </summary>
        public EntityRequestInput()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityRequestInput"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityRequestInput(Guid id)
            : base(id)
        {

        }
    }
}