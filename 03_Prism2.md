# 

##

## コードビハインド

### Bindingを強制的に評価

```C#
  private void Button_Click(object sender, RoutedEventArgs e)
  {
    var expression = this.e.GetBindingExpression(TextBox.TextProperty);
    expression.UpdateTarget();
  }
```

