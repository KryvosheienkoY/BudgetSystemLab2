using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSystemLab2
{
    public class Category : EntityBase, IStorable
    {

        private Guid _guid;
        private string _name;
        private string _description;
        private Enums.Colors _color;
        private string _categoryIconPath;
        private Guid _userId;


        public string CategoryIconPath { get => _categoryIconPath; set => _categoryIconPath = value; }
        public Enums.Colors Color { get => _color; set => _color = value; }
        public string Description { get => _description; set => _description = value; }
        public string Name { get => _name; set => _name = value; }
        public Guid Guid { get => _guid; private set => _guid = value; }
        public Guid UserId { get => _userId; private set => _userId = value; }


        public Category(Guid userId)
        {
            _guid = Guid.NewGuid();
            _userId = userId;
        }
        public Category(Guid guid, Guid userId, string name, string description, Enums.Colors color, string icon)
        {
            _guid = guid;
            _userId = userId;
            _name = name;
            _description = description;
            _color = color;
            _categoryIconPath = icon;

        }

        public override bool Validate()
        {
            return (Guid!=Guid.Empty) && (UserId != Guid.Empty) && !String.IsNullOrWhiteSpace(Name);
        }
    }
}
