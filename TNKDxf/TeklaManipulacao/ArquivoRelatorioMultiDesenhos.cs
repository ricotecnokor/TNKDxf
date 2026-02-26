using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNKDxf.TeklaManipulacao
{
    public class ArquivoRelatorioMultiDesenhos
    {
        const string CONTEUDO = @"
template 
{
    name = ""template_319"";
    type = TEXTUAL;
    width = 341;
    maxheight = 5;
    columns = (1, 1);
    gap = 1;
    fillpolicy = EVEN;
    filldirection = HORIZONTAL;
    fillstartfrom = TOPLEFT;
    margins = (4, 0, 0, 0);
    gridxspacing = 1;
    gridyspacing = 1;
    version = 3.21;
    created = ""20.01.2009 22:32"";
    modified = ""26.02.2026 09:41"";
    notes = ""Converted template"";

    header 
    {
        name = ""header_2534"";
        height = 1;

        text _tmp_37
        {
            name = ""Texto_31"";
            x1 = 66;
            y1 = 0;
            x2 = 66;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        valuefield _tmp_38
        {
            name = ""NOME_PROJETO"";
            location = (69, 0);
            formula = ""GetValue(\""PROJECT.NUMBER\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 14;
            decimals = 0;
            sortdirection = NONE;
            fontname = ""Arial"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 5;
            fontratio = 1.5;
            fontstyle = 0;
            fontslant = 0;
            pen = -1;
            oncombine = NONE;
            aligncontenttotop = FALSE;
        };

        text _tmp_39
        {
            name = ""Texto"";
            x1 = 85;
            y1 = 0;
            x2 = 85;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        valuefield _tmp_40
        {
            name = ""HORA"";
            location = (104, 0);
            formula = ""GetValue(\""TIME\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 10;
            decimals = 0;
            sortdirection = NONE;
            fontname = ""Arial"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 5;
            fontratio = 1.5;
            fontstyle = 0;
            fontslant = 0;
            pen = -1;
            oncombine = NONE;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_41
        {
            name = ""DATA"";
            location = (87, 0);
            formula = ""GetValue(\""DATE\"")"";
            maxnumoflines = 1;
            datatype = INTEGER;
            class = ""Date"";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 11;
            decimals = 0;
            sortdirection = NONE;
            fontname = ""Arial"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 5;
            fontratio = 1.5;
            fontstyle = 0;
            fontslant = 0;
            pen = -1;
            oncombine = NONE;
            unit = ""dd.mm.yyyy"";
            aligncontenttotop = FALSE;
        };

        text _tmp_42
        {
            name = ""Texto_4"";
            x1 = 101;
            y1 = 0;
            x2 = 101;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        valuefield _tmp_43
        {
            name = ""PROJECT.MODEL"";
            location = (14, 0);
            formula = ""GetValue(\""PROJECT.MODEL\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 50;
            decimals = 0;
            sortdirection = NONE;
            fontname = ""Arial"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 5;
            fontratio = 1.5;
            fontstyle = 0;
            fontslant = 0;
            pen = -1;
            oncombine = NONE;
            aligncontenttotop = FALSE;
        };

        text _tmp_45
        {
            name = ""Texto_32"";
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            string = ""CABECALHO"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_48
        {
            name = ""Texto_33"";
            x1 = 11;
            y1 = 0;
            x2 = 11;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };
    };

    pageheader _tmp_924
    {
        name = ""CabeçalhoPágina"";
        height = 1;
        outputpolicy = NONE;

        text _tmp_108
        {
            name = ""Texto_12"";
            x1 = 330;
            y1 = 0;
            x2 = 330;
            y2 = 0;
            string = ""ESCALA"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_109
        {
            name = ""Texto_11"";
            x1 = 309;
            y1 = 0;
            x2 = 309;
            y2 = 0;
            string = ""REVISÃO_CLIENTE"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_110
        {
            name = ""Texto_10"";
            x1 = 298;
            y1 = 0;
            x2 = 298;
            y2 = 0;
            string = ""REVISÃO"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_111
        {
            name = ""Texto_19"";
            x1 = 297;
            y1 = 0;
            x2 = 297;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_112
        {
            name = ""Texto_20"";
            x1 = 307;
            y1 = 0;
            x2 = 307;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_113
        {
            name = ""Texto_21"";
            x1 = 327;
            y1 = 0;
            x2 = 327;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_114
        {
            name = ""Texto_18"";
            x1 = 256;
            y1 = 0;
            x2 = 256;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_115
        {
            name = ""Texto_17"";
            x1 = 214;
            y1 = 0;
            x2 = 214;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_116
        {
            name = ""Texto_16"";
            x1 = 172;
            y1 = 0;
            x2 = 172;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_117
        {
            name = ""Texto_15"";
            x1 = 120;
            y1 = 0;
            x2 = 120;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_118
        {
            name = ""Texto_14"";
            x1 = 36;
            y1 = 0;
            x2 = 36;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_119
        {
            name = ""Texto_13"";
            x1 = 68;
            y1 = 0;
            x2 = 68;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_120
        {
            name = ""Texto_9"";
            x1 = 258;
            y1 = 0;
            x2 = 258;
            y2 = 0;
            string = ""SUBTÍTULO_2_DO_DESENHO"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_121
        {
            name = ""Texto_8"";
            x1 = 216;
            y1 = 0;
            x2 = 216;
            y2 = 0;
            string = ""SUBTÍTULO_1_DO_DESENHO"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_122
        {
            name = ""Texto_7"";
            x1 = 176;
            y1 = 0;
            x2 = 176;
            y2 = 0;
            string = ""TÍTULO_DO_DESENHO"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_123
        {
            name = ""Texto_5"";
            x1 = 124;
            y1 = 0;
            x2 = 124;
            y2 = 0;
            string = ""ÁREA_E/OU_SUBÁREA"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_124
        {
            name = ""Texto_3"";
            x1 = 70;
            y1 = 0;
            x2 = 70;
            y2 = 0;
            string = ""DECRICAO_DO_PROJETO"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_125
        {
            name = ""Texto_2"";
            x1 = 38;
            y1 = 0;
            x2 = 38;
            y2 = 0;
            string = ""NUMERO_CLIENTE"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_126
        {
            name = ""Texto_1"";
            x1 = 14;
            y1 = 0;
            x2 = 14;
            y2 = 0;
            string = ""NÚMERO_DA_CONTRATADA"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_129
        {
            name = ""Texto_34"";
            x1 = 13;
            y1 = 0;
            x2 = 13;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_130
        {
            name = ""Texto_35"";
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            string = ""PROPRIEDADES"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };
    };

    row 
    {
        name = ""row_2535"";
        height = 1;
        visibility = TRUE;
        usecolumns = FALSE;
        rule = ""if GetValue(\""TYPE\"") == \""M\"" then\n Output()\nelse\n  StepOver()\nendif"";
        contenttype = ""DRAWING"";
        sorttype = COMBINE;

        text _tmp_191
        {
            name = ""Texto_36"";
            x1 = 13;
            y1 = 0;
            x2 = 13;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_192
        {
            name = ""Texto_30"";
            x1 = 327;
            y1 = 0;
            x2 = 327;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_193
        {
            name = ""Texto_29"";
            x1 = 307;
            y1 = 0;
            x2 = 307;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_194
        {
            name = ""Texto_28"";
            x1 = 297;
            y1 = 0;
            x2 = 297;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_195
        {
            name = ""Texto_27"";
            x1 = 256;
            y1 = 0;
            x2 = 256;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_196
        {
            name = ""Texto_26"";
            x1 = 214;
            y1 = 0;
            x2 = 214;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_197
        {
            name = ""Texto_25"";
            x1 = 172;
            y1 = 0;
            x2 = 172;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_198
        {
            name = ""Texto_24"";
            x1 = 120;
            y1 = 0;
            x2 = 120;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_199
        {
            name = ""Texto_23"";
            x1 = 68;
            y1 = 0;
            x2 = 68;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        text _tmp_200
        {
            name = ""Texto_22"";
            x1 = 36;
            y1 = 0;
            x2 = 36;
            y2 = 0;
            string = "";"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };

        valuefield _tmp_201
        {
            name = ""ESCALA"";
            location = (330, 0);
            formula = ""\""1:\"" + max(int(mid(GetValue(\""SCALE1\""),2,3)),\nint(mid(GetValue(\""SCALE2\""),2,3)),\nint(mid(GetValue(\""SCALE3\""),2,3)),\nint(mid(GetValue(\""SCALE4\""),2,3)),\nint(mid(GetValue(\""SCALE5\""),2,3)))"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 7;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_202
        {
            name = ""REVISÃO_CLIENTE"";
            location = (309, 0);
            formula = ""GetValue(\""REVISION.LAST_MARK\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 16;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_203
        {
            name = ""REVISÃO"";
            location = (298, 0);
            formula = ""GetValue(\""REVISION.LAST_MARK\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 8;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_204
        {
            name = ""SUBTÍTULO_2_DO_DESENHO"";
            location = (258, 0);
            formula = ""GetValue(\""PROJECT.OBJECT\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 38;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_205
        {
            name = ""SUBTÍTULO_1_DO_DESENHO"";
            location = (216, 0);
            formula = ""GetValue(\""PROJECT.OBJECT\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 38;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_206
        {
            name = ""TÍTULO_DO_DESENHO"";
            location = (175, 0);
            formula = ""GetValue(\""PROJECT.OBJECT\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 38;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_207
        {
            name = ""ÁREA_E/OU_SUBÁREA"";
            location = (123, 0);
            formula = ""GetValue(\""PROJECT.OBJECT\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 48;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_208
        {
            name = ""DECRICAO_DO_PROJETO"";
            location = (70, 0);
            formula = ""GetValue(\""PROJECT.OBJECT\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 48;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_209
        {
            name = ""NÚMERO_DA_CONTRATADA"";
            location = (14, 0);
            formula = ""GetValue(\""TITLE1\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 18;
            decimals = 0;
            sortdirection = NONE;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        valuefield _tmp_210
        {
            name = ""NUMERO_CLIENTE"";
            location = (37, 0);
            formula = ""GetValue(\""TITLE2\"")"";
            maxnumoflines = 1;
            datatype = STRING;
            class = """";
            cacheable = TRUE;
            formatzeroasempty = FALSE;
            justify = LEFT;
            visibility = TRUE;
            angle = 0;
            length = 30;
            decimals = 0;
            sortdirection = ASCENDING;
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontstyle = 0;
            fontslant = 0;
            pen = 0;
            oncombine = NONE;
            fontlinewidth = 1;
            aligncontenttotop = FALSE;
        };

        text _tmp_212
        {
            name = ""Texto_37"";
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            string = ""VALORES"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };
    };

    footer _tmp_1677
    {
        name = ""Rodapé"";
        height = 1;

        text _tmp_50
        {
            name = ""Texto_6"";
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            string = ""RODAPE"";
            fontname = ""Courier New"";
            fontcolor = 153;
            fontcolor_argb = """";
            fonttype = 2;
            fontsize = 0.8;
            fontratio = 1.125;
            fontslant = 0;
            fontstyle = 0;
            angle = 0;
            justify = LEFT;
            pen = 0;
            fontlinewidth = 1;
            isalwaysvisible = TRUE;
            visibilityrule = """";
        };
    };
};
";
        private readonly string _caminho = string.Empty;  //= @"C:\APP\TNKDxfListaMULTI.rpt";
        const string DIRETORIO = @"C:\ProgramData\TNK\Relatorios";
        public ArquivoRelatorioMultiDesenhos(string nomeArquivo)
        {

            if(!Directory.Exists(DIRETORIO)) Directory.CreateDirectory(DIRETORIO);
            _caminho = Path.Combine(DIRETORIO, nomeArquivo);

        }

        public string Caminho => _caminho;

        public void CriarArquivoRelatorio() 
        {
            System.IO.File.WriteAllText(_caminho, CONTEUDO);

        }

        public void DeleteArquivo(string nomeArquivo)
        {
            string caminhoArquivo = Path.Combine(DIRETORIO, nomeArquivo);
            if (File.Exists(caminhoArquivo))
            {
                File.Delete(caminhoArquivo);
            } 
        }
    }
}
