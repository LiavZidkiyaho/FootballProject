using System;

namespace FootballProject.Model
{
    /// <summary>
    /// Delegate for creating an OleDb-compatible SQL command from a given BaseEntity.
    /// </summary>
    /// <param name="entity">The entity to convert into a SQL statement.</param>
    /// <returns>A string containing the OleDb SQL command.</returns>
    public delegate string CreateOleDb(BaseEntity entity);

    /// <summary>
    /// Represents a change operation that includes an entity and a method to convert it to an OleDb command.
    /// </summary>
    public class ChangeEntity
    {
        private BaseEntity entity;
        private CreateOleDb createOleDb;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEntity"/> class.
        /// </summary>
        /// <param name="createOleDb">A delegate that generates an OleDb SQL command from the entity.</param>
        /// <param name="entity">The entity to be tracked for changes.</param>
        public ChangeEntity(CreateOleDb createOleDb, BaseEntity entity)
        {
            this.createOleDb = createOleDb;
            this.entity = entity;
        }

        /// <summary>
        /// Gets or sets the entity involved in the change operation.
        /// </summary>
        public BaseEntity Entity
        {
            get => entity;
            set => entity = value;
        }

        /// <summary>
        /// Gets or sets the delegate responsible for generating the OleDb SQL command.
        /// </summary>
        public CreateOleDb CreateOleDb
        {
            get => createOleDb;
            set => createOleDb = value;
        }
    }
}
