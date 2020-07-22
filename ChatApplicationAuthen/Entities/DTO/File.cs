﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplicationAuthen.Entities.DTO
{
    public class File
    {
        public Guid Id { get; set; }
        public string convId { get; set; }
        public string content { get; set; }
        public string filePath { get; set; }
        public int type { get; set; }
        public string typeOf { get; set; }
    }
}
