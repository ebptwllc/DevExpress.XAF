﻿using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Utils;
using Xpand.Extensions.Reactive.Transform;

namespace Xpand.XAF.Modules.Reactive.Services{
    public static class BoolListExtensions{
        public static IObservable<(BoolList boolList, BoolValueChangedEventArgs e)> WhenResultValueChanged(
            this BoolList source){
            return Observable.Return(source).ResultValueChanged();
        }

        public static IObservable<(BoolList boolList, BoolValueChangedEventArgs e)> ResultValueChanged(
            this IObservable<BoolList> source){
            return source
                .SelectMany(item => {
                    return Observable.FromEventPattern<EventHandler<BoolValueChangedEventArgs>, BoolValueChangedEventArgs>(
                            h => item.ResultValueChanged += h, h => item.ResultValueChanged -= h,
                            ImmediateScheduler.Instance);
                })
                .Select(pattern => pattern)
                .TransformPattern<BoolValueChangedEventArgs, BoolList>();
        }
    }
}