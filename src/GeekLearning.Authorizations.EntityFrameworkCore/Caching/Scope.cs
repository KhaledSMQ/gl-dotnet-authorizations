﻿namespace GeekLearning.Authorizations.EntityFrameworkCore.Caching
{
    using System;
    using System.Collections.Generic;

    public class Scope
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Guid> ParentIds { get; set; }

        public IEnumerable<Guid> ChildIds { get; set; }
    }
}
