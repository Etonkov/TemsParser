   M S B A M L   `   `   `  ����E  ATemsParser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TemsParser
TemsParser  5      K*clr-namespace:TemsParser.ViewModels.ConfigTemsParser.ViewModels.Config  5      =#clr-namespace:TemsParser.ViewModelsTemsParser.ViewModels  5      3clr-namespace:TemsParser.ViewsTemsParser.Views  5          TemsParser.App   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      *localclr-namespace:TemsParser     5      GconfigViewModels*clr-namespace:TemsParser.ViewModels.Config     5      :
viewModels#clr-namespace:TemsParser.ViewModels     5      0viewsclr-namespace:TemsParser.Views     5        �� ShutdownMode$  OnMainWindowClose=�5       �� 	Resources 5
       �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value �� RelativeSource	 �� Path
 �� DataType2   ,TemsParser.ViewModels.Config.ConfigViewModel!   TemsParser.Views.ConfigView1   +TemsParser.ViewModels.CalcDistanceViewModel'   !TemsParser.Views.CalcDistanceView0   *TemsParser.ViewModels.Config.FreqViewModel   TemsParser.Views.FreqView5   /TemsParser.ViewModels.Config.INameItemViewModel$   TemsParser.Views.INameItemView,	   &TemsParser.ViewModels.MessageViewModel"
   TemsParser.Views.MessageView,   &TemsParser.ViewModels.ParsingViewModel"   TemsParser.Views.ParsingView>   8TemsParser.ViewModels.Config.TechnologyListItemViewModel-   'TemsParser.Views.TechnologyListItemView5   /TemsParser.ViewModels.Config.INameListViewModel$   TemsParser.Views.INameListView9   3TemsParser.ViewModels.Config.INameListItemViewModel(   "TemsParser.Views.INameListItemView��%�  '��       '�� �     '� �    '�� d    (�� 0    5/      *, +)(�� b    52      *, +)(�� �    55      *, +)(�� �    58      *, +)(�� �    5;      *,	 +)(�� *    5>      *, +)(�� \    5A      *, +)(�� �    5D      *, +)(�� �    5G      *, +)�� 5   
   " ��6    5      P� 5       �@=�HasError . . �5      ��  �@�ToolTip 5      ��  �� *Self+$)	 #(Validation.Errors)[0].ErrorContent�5      5      �� 5   
   " ��6    5      P� 5       �@=�HasError . . �5      ��  �@�ToolTip 5      ��  �� *Self+$)	 #(Validation.Errors)[0].ErrorContent�5      5      �� 5   
   " �6    5      P� 5       �@=�HasError . . �5       ��  �@�ToolTip 5!      ��  �� *Self+$)	 #(Validation.Errors)[0].ErrorContent�5#      5%      �� 5&   
   " ��6    5'      P� 5(       �@=�HasError . . �5)      ��  �@�ToolTip 5*      ��  �� *Self+$)	 #(Validation.Errors)[0].ErrorContent�5,      5.      �� 5/   
   "
  6   .R�50        51      �� 52   
   "
  6   .R�53        54      �� 55   
   "
  6   .R�56        57      �� 58   
   "
  6   .R�59        5:      �� 5;   
   "
 	 6   .R�5<      
  5=      �� 5>   
   "
  6   .R�5?        5@      �� 5A   
   "
  6   .R�5B        5C      �� 5D   
   "
  6   .R�5E        5F      �� 5G   
   "
  6   .R�5H        5I      5J      5K      