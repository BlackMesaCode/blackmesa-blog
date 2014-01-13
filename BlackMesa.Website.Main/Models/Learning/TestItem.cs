﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Table("Learning_TestItems")]
    public class TestItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public TestResult Result { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [NotMapped]
        public TimeSpan Duration { get { return EndTime - StartTime; } }

        public Guid CardId { get; set; }

        public virtual Card Card { get; set; }

        public string TestId { get; set; }
    }

    public enum TestResult
    {
        [Display(Name = "TestResultCorrect", ResourceType = typeof(Strings))]
        Correct,
        [Display(Name = "TestResultPartlyCorrect", ResourceType = typeof(Strings))]
        PartlyCorrect,
        [Display(Name = "TestResultWrong", ResourceType = typeof(Strings))]
        Wrong,
    }
}