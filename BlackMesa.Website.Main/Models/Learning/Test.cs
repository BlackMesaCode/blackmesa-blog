﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Test;
using BlackMesa.Website.Main.Models.Identity;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Models.Learning
{
    [Serializable]
    public class Test
    {
        public Guid Id { get; set; }

        public bool TestOnlyDueCards { get; set; }
        public bool ReverseSides { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Order")]
        public OrderType OrderType { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "TestType")]
        public TestType TestType { get; set; }

        public List<Card> CardsToTest { get; set; }

        public TestStatus TestStatus { get; set; }

        public DateTime StartTime { get; set; }
    }

    public enum TestStatus
    {
        InProgress,
        Paused,
        Completed,
        Aborted,
    }

}