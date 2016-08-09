#include <stdio.h>

int main()
{
    int var[8][8]={0};
    float U[8];
    int xip[8],xim[8];
    int i,j;
    int xx,yy,z;

    var[2][1]=var[2][3]=var[2][4]=var[2][7]=1;
    var[3][4]=var[3][7]=1;
    var[4][5]=var[4][6]=var[4][7]=1;
    var[5][2]=1;
    var[6][1]=var[6][4]=1;
    var[7][6]=1;

    printf("Матрица отношений по варианту:\n");

    for(i=1; i<=7; i++)
    {
        for(j=1; j<=7; j++)
            printf("%d ",var[i][j]);
        printf("\n");
    }
    printf("\nx1\n");
    printf("U(x1)=0.0\n\n");
    U[1]=0.0;
    for(int k=2; k<=7; k++)
    {
        printf("x%d\n",k);
        int p=0,m=0;
        for(i=1; i<=k; i++)
            {
                if(var[k][i] == 1)
                {
                    m++;
                    xim[m]=i;
                }
                if(var[i][k] == 1)
                {
                    p++;
                    xip[p]=i;
                }
            }
        printf("xip={ ");
        for(int tt=1; tt<=p; tt++)
            printf("%d ",xip[tt]);
        printf("}\n");
        printf("xim={ ");
        for(int tt=1; tt<=m; tt++)
            printf("%d ",xim[tt]);
        printf("}\n");
        if(m == 0 && p > 0)
        {

            U[k]=U[xip[p]]-1.0;
        }
        if(p == 0 && m > 0)
        {
            U[k]=U[xim[m]]+1.0;
        }
        if(p >0 && m > 0)
        {
            int f=0,uu;
            for(xx=1; xx<=p; xx++)
            {
                if(f == 0)
                for(yy=1; yy<=m; yy++)
                    if(xip[xx] == xim[yy])
                    {
                        f=1;
                        uu=xip[xx];
                        break;
                    }
            }
            if(f == 1)
            {
                U[k]=U[uu];
            }
            else
            {
                U[k]=(U[xip[p]]+U[xim[m]])/2.0;
            }
        }
        printf("U(x%d)=%.2f ",k,U[k]);
        printf("\n\n");

    }
    int max=1;
    for(i=2; i<=7; i++)
        if(U[i]>U[max])
            max=i;
    printf("x* = %d\n",max);
    printf("U(x*) = %.2f",U[max]);


    return 0;
}
