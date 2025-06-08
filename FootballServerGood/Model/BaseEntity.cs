using System;

namespace FootballServerGood.Model
{
    /// <summary>
    /// Represents the abstract base class for all entities in the database.
    /// Provides a common <c>Id</c> property for uniquely identifying records.
    /// </summary>
    public class BaseEntity
    {
        private int id;

        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// This property typically maps to the primary key in the database.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
