#include <stdio.h>

int main()
{
    int var[6][6]={0};
    int var_zad5_1[5][5]={0},var_zad5_2[5][5]={0};
    int MaxR[6],MaxR_1[5],MaxR_2[5];
    int i,j;

    MaxR[63]=1;
    for(i=0; i<6; i++)
        MaxR[i]=1;
    for(i=0; i<5; i++)
        MaxR_1[i]=1;
    for(i=0; i<5; i++)
        MaxR_2[i]=1;

    var[0][1]=var[1][0]=var[1][3]=var[3][1]=var[1][2]=var[3][4]=var[5][4]=1;

    var_zad5_1[0][3]=var_zad5_1[3][2]=var_zad5_1[2][3]=var_zad5_1[1][2]=var_zad5_1[2][4]=var_zad5_1[4][2]=1;

    var_zad5_2[0][1]=var_zad5_2[1][0]=var_zad5_2[1][2]=var_zad5_2[2][1]=var_zad5_2[2][3]=var_zad5_2[3][2]=var_zad5_2[0][3]=var_zad5_2[2][4]=1;

    printf("Матрица отношений по варианту:\n");

    for(i=0; i<6; i++)
    {
        for(j=0; j<6; j++)
            printf("%d ",var[i][j]);
        printf("\n");
    }
    for(i=0; i<6; i++)
        for(j=0; j<6; j++)
            if (var[i][j]==1)
                if (var[j][i]==0)
                    MaxR[j]=0;
                    else
                if (var[j][i]==1 && MaxR[i]==0)
                    MaxR[j]=0;
    printf("\nMaxR:\n");
    for(j=0; j<6; j++)
            printf("%d ",MaxR[j]);

    //----------------------------------------------------------------------------
    printf("\n\nПример 5 вариант 1:\n");
    printf("Матрица отношений к заданиу 5.1:\n");
    for(i=0; i<5; i++)
    {
        for(j=0; j<5; j++)
            printf("%d ",var_zad5_1[i][j]);
        printf("\n");
    }
    for(i=0; i<5; i++)
        for(j=0; j<5; j++)
            if (var_zad5_1[i][j]==1)
                if (var_zad5_1[j][i]==0)
                    MaxR_1[j]=0;
                    else
                if (var_zad5_1[j][i]==1 && MaxR_1[i]==0)
                    MaxR_1[j]=0;
    printf("\nMaxR_1:\n");
    for(j=0; j<5; j++)
            printf("%d ",MaxR_1[j]);


    //------------------------------------------------------------------------------
    printf("\n\nПример 5 вариант 2:\n");
    printf("Матрица отношений к заданиу 5.2:\n");
    for(i=0; i<5; i++)
    {
        for(j=0; j<5; j++)
            printf("%d ",var_zad5_2[i][j]);
        printf("\n");
    }
    for(i=0; i<5; i++)
        for(j=0; j<5; j++)
            if (var_zad5_2[i][j]==1)
                if (var_zad5_2[j][i]==0)
                    MaxR_2[j]=0;
                    else
                if (var_zad5_2[j][i]==1 && MaxR_2[i]==0)
                    MaxR_2[j]=0;
    printf("\nMaxR_2:\n");
    for(j=0; j<5; j++)
            printf("%d ",MaxR_2[j]);

    return 0;
}
