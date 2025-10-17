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
5) O XAML já está configurado com:
   <FontFamily x:Key="Font.Primary">/TNKDxf;component/Fonts/#Montserrat</FontFamily>
   Se você quiser especificar um weight exato (quando tiver variações), use o mesmo family name.

Observações:
- O family name interno precisa ser "Montserrat" (o que normalmente é para os .ttf oficiais).
- Se notar que não renderiza, verifique o family name real do arquivo (Properties do .ttf) e ajuste a entrada no App.xaml se necessário.
