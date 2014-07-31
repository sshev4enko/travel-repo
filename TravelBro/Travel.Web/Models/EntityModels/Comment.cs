﻿using System;
using WebApplication1.Models.IdentityModels;

namespace WebApplication1.Models.EntityModels
{
    public class Comment
    {
        public int Id { get; set; }

        public DateTime Published { get; set; }
        public string Text { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Visit Visit { get; set; }
    }
}