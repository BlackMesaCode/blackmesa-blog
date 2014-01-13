﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Resources;

namespace BlackMesa.Website.Main.Areas.Learning.ViewModels.Test
{
    public class SetupTestViewModel
    {
        public string FolderId { get; set; }

        public bool TestOnlyDueCards { get; set; }
        public bool ReverseSides { get; set; }


        [Display(ResourceType = typeof(Strings), Name = "Order")]
        public OrderType OrderType { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "TestType")]
        public TestType TestType { get; set; }

    }


    public enum OrderType
    {
        [Display(Name = "OrderTypeOrdered", ResourceType = typeof(Strings))]
        Ordered,
        [Display(Name = "OrderTypeShuffled", ResourceType = typeof(Strings))]
        Shuffled,
    }

    public enum TestType
    {
        [Display(Name = "TestTypeNormal", ResourceType = typeof(Strings))]
        Normal,
        [Display(Name = "TestTypeSinglePass", ResourceType = typeof(Strings))]
        SinglePass,
    }

}