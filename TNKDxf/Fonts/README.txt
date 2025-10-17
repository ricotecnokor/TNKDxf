README - Fontes

Para incorporar a fonte Montserrat e evitar fallback do sistema:

1) Baixe os arquivos de fonte Montserrat (recomendado Regular, Medium, SemiBold, Bold) do Google Fonts.
2) Crie a pasta: TNKDxf/Fonts
3) Copie os arquivos .ttf para essa pasta. Exemplos sugeridos:
   - Montserrat-Regular.ttf
   - Montserrat-Medium.ttf
   - Montserrat-SemiBold.ttf
   - Montserrat-Bold.ttf
4) Compile o projeto. O MSBuild vai embutir automaticamente como Resource via Directory.Build.props.
5) O XAML j� est� configurado com:
   <FontFamily x:Key="Font.Primary">/TNKDxf;component/Fonts/#Montserrat</FontFamily>
   Se voc� quiser especificar um weight exato (quando tiver varia��es), use o mesmo family name.

Observa��es:
- O family name interno precisa ser "Montserrat" (o que normalmente � para os .ttf oficiais).
- Se notar que n�o renderiza, verifique o family name real do arquivo (Properties do .ttf) e ajuste a entrada no App.xaml se necess�rio.
