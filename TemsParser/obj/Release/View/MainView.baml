   M S B A M L   `   `   `  ���� E  ATemsParser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:TemsParser.SupportMvvmTemsParser.SupportMvvm      TemsParser.View.MainView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     7custom$clr-namespace:TemsParser.SupportMvvm       � Title$  
TemsParser�� � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=�#��� �� � 	Resources ��%    .���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�o� $
��Left=�$
��auto��$��0��$	��Top=�$
��auto��.@�m� $��ФАЙЛ��.@�m� $��Открыть�� m� InputGestureText$ Ctrl+O�� m� Command �� *OpenButtonCommand+�� m� $��
Выход�� �� *ExitButtonCommand+m� $��КОНФИГУРАЦИЯ��.@�m� $��Импорт��$ Ctrl+I��m� $��Экспорт��$ Ctrl+E���� m� $��Настройка��$ Ctrl+P��m� $-��'ВЫЧИСЛИТЬ РАССТОЯНИЕ�� �� * CalculateDistanceButtonCommand+�� $5��/Выбор файла для обработки��$��5q�$��1��.���� .+��� $��Открыть��$��75��$
��Left=����� *OpenButtonCommand+-   'TemsParser.SupportMvvm.TextBoxSelectAll  -     Name$ openFileTextBox��$
��Left=�$��21�� �� 
IsReadOnly . $	��Top=�$��0q�#��� ��*	 �� #IsInactiveSelectionHighlightEnabled	 .  ���� 
 �� Path$
 Settings.LastOpenFile� �� Mode$ OneWay=� �� UpdateSourceTrigger$ PropertyChanged=�.���� $+��%Параметры обработки��$��5q�$��2��.���� .+��� $
��Left=�$
��Wrap=�$��Регион��$	��Top=��� -    ��Name$ regionComboBox��$
��Left=�$	��100���� $
��Left=�$��0,5,0,0q�$
��Wrap=�$��Операторы��$	��Top=�� -    �Name$ operatorsWrapPanel��$��25���� $
��Left=�$��0,5,0,0q�$
��Wrap=�$��Технологии��$	��Top=�� -   $ technologyWrapPanel��$��25���� -    ��Name$ configButton��$��Конфигурация��$��90��$
��Left=�$��0,5,0,0q����� *ConfigButtonCommand+�� $)��#Параметры биннинга��$��5q�$��3��.��� .+��� -    ��Name$ binningCheckBox��$��Биннинг�� d� 	IsChecked �� $
 BinningEnabled�$ PropertyChanged=���  � Minimum$ 1Q� � Maximum$	 500Q� �� TickPlacement$ BottomRight=� �� TickFrequency$ 50Q�$
��Left=�$��5,0,0,0q�$	��Top=�$	��200��{��� $
 BinningEnabled�$ PropertyChanged=� � Value �� $
 BinningSizeSlider�$ PropertyChanged=��� $��23��$��5,0,0,0q�$��30��{��� $
 BinningEnabled�$ PropertyChanged=�.������ $
 BinningSizeTextBox�$ PropertyChanged=� �� NotifyOnValidationError .  �� ValidationRules 5  /System.Windows.Controls.DataErrorValidationRule  .���� $
��Left=�$
��Wrap=� �� Padding$	 0,3q�$��м��$	��Top=��� $��Выполнение��$��5q�$��4��.���� .+�� -    �Name$ runParsingProgressBar��$��0,0,0,5q�$��20��$
��auto���� -   $ runParsingButton��$��Выполнить��$
��Left=�$	��Top=�$��75��