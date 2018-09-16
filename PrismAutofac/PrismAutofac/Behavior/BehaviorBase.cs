using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Behavior
{
    public class BehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {
        /// <summary>
        /// セットアップ状態
        /// </summary>
        private bool isSetup = false;

        /// <summary>
        /// Hook状態
        /// </summary>
        private bool isHookedUp = false;

        /// <summary>
        /// 対象オブジェクト
        /// </summary>
        private WeakReference weakTarget;

        /// <summary>
        /// Changedハンドラ
        /// </summary>
        protected override void OnChanged()
        {
            base.OnChanged();


            //==== 関連オブジェクト有無判定 ====//
            var target = AssociatedObject;
            if (target != null)
            {
                //-==- 有り -==-//

                //==== Hook開始 ====//
                HookupBehavior(target);
            }
            else
            {
                //-==- 無し -==-//

                //==== Hook解除 ====//
                UnHookupBehavior();
            }
        }


        /// <summary>
        /// ビヘイビアをHookする。
        /// </summary>
        /// <param name="target"></param>
        private void HookupBehavior(T target)
        {
            //==== Hook状態判定 ====//
            if (isHookedUp)
            {
                //-==- Hook中 -==-//
                return;
            }


            //==== Hook開始 ====//
            weakTarget = new WeakReference(target);
            isHookedUp = true;
            target.Unloaded += OnTargetUnloaded;
            target.Loaded += OnTargetLoaded;


            //==== ビヘイビアのセットアップ ====//
            SetupBehavior();
        }


        /// <summary>
        /// ビヘイビアをUnhookする。
        /// </summary>
        private void UnHookupBehavior()
        {
            //==== Hook状態判定 ====//
            if (!isHookedUp)
            {
                //-==- 未Hook -==-//
                return;
            }


            //==== Hook解除 ====//
            isHookedUp = false;
            var target = AssociatedObject ?? (T)weakTarget.Target;
            if (target != null)
            {
                target.Unloaded -= OnTargetUnloaded;
                target.Loaded -= OnTargetLoaded;
            }


            //==== ビヘイビアのクリーンアップ ====//
            CleanupBehavior();
        }


        /// <summary>
        /// [関連オブジェクト] Loadedハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTargetLoaded(object sender, RoutedEventArgs e)
        {
            //==== ビヘイビアのセットアップ ====//
            SetupBehavior();
        }


        /// <summary>
        /// [関連オブジェクト] Unloadedハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTargetUnloaded(object sender, RoutedEventArgs e)
        {
            //==== ビヘイビアのクリーンアップ ====//
            CleanupBehavior();
        }


        /// <summary>
        /// セットアップ時の処理を行う。
        /// </summary>
        protected virtual void OnSetup() { }


        /// <summary>
        /// クリーンアップ時の処理を行う。
        /// </summary>
        protected virtual void OnCleanup() { }


        /// <summary>
        /// ビヘイビアのセットアップを行う。
        /// </summary>
        private void SetupBehavior()
        {
            if (isSetup)
            {
                return;
            }

            isSetup = true;
            OnSetup();
        }


        /// <summary>
        /// ビヘイビアのクリーンアップを行う。
        /// </summary>
        private void CleanupBehavior()
        {
            if (!isSetup)
            {
                return;
            }

            isSetup = false;
            OnCleanup();
        }

    }
}
