using Prism.Commands;
using Prism.Mvvm;
using PrismAutofac.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrismAutofac.ViewModels
{
    public class ConfigurationViewModel : BindableBase
	{
        public enum TestEnum
        {
            A,
            B
        }

        public ReactiveProperty<List<TestEnum>> b { get; private set; }

        public ReactiveProperty<object> obj { get; private set; }
        public ReactiveProperty<string> list { get; private set; }
        public ReactiveProperty<TestEnum> a { get; private set; }

        public ConfigurationViewModel(ModelBase model)
        {
            a = new ReactiveProperty<TestEnum>(TestEnum.A);

            var c = (Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>()).ToList();
            b = new ReactiveProperty<List<TestEnum>>(c);

            obj = new ReactiveProperty<object>((object)model);

            list = new ReactiveProperty<string>();

            a.Subscribe(x =>
            {
                switch (x)
                {
                    case TestEnum.A:
                        list.Value = File.ReadAllText("A.json");
                        break;
                    case TestEnum.B:
                        list.Value = File.ReadAllText("B.json");
                        break;
                    default:
                        break;
                }
            });

        }
    }
}
