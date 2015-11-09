namespace Skymate.Application.Dto
{
    using System;

    using Skymate.Application;

    /// <summary>
    /// This <see cref="IOutputDto"/> can be used to send Id of an entity as response from an <see cref="IApplicationService"/> method.
    /// </summary>
    [Serializable]
    public class EntityResultOutput : EntityResultOutput<Guid>, IEntityDto
    {
        /// <summary>
        /// Creates a new <see cref="EntityResultOutput"/> object.
        /// </summary>
        public EntityResultOutput()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityResultOutput"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityResultOutput(Guid id)
            : base(id)
        {

        }
    }
}