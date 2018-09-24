# Prism Autofac Boilerplate

## Description

Atom的ななにがしのUIっぽく（tabつけてないけど）


## Image関連のデザインの提案

- 画像が主役 = 画面は目一杯広く使いたい
  - 固定されたmenuやstatusbarは使用しない
  - popupからの遷移を明確にする
  - 画面を隠すことは望まれない = 位置の変更化
  - Shortcutコマンドの有効活用
- 設定が広範囲
  - 外部ファイルからの注入と必要操作範囲の限定
  - Jsonでの外部注入
  - Formのカスタム/自動生成
- 設定がわからないと再現性が
  - MVVMで分離・オブジェクトのシリアライズ
  - JSONでの依存性注入
