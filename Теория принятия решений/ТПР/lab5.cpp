#include <iostream>
#include <fstream>
#include <iomanip>
#include <string>
#include <math.h>

using namespace std;

const int N=10;

struct reshenie{
    int f1,f2;
    bool inPareto;
};

bool n_r(reshenie a,reshenie b)
{
    if( a.f1 < b.f1 && a.f2 >= b.f2 ||
        a.f1 >= b.f1 && a.f2 < b.f2 ||
        a.f1 <= b.f1 && a.f2 > b.f2 ||
        a.f1 > b.f1 && a.f2 <= b.f2 )
        return true;
    else
        return false;
}

bool b_r(reshenie a,reshenie b)
{
    if(a.f1 > b.f1 && a.f2 >= b.f2 ||
       a.f1 >= b.f1 && a.f2 > b.f2)
        return true;
    else
        return false;
}


int main(){
    reshenie matrix[N];
    int i;
    int t;
    ifstream k("5.txt", ios::in);
	for (i = 0; i < N; i++)
        matrix[i].inPareto = false;

	for (i = 0; i < N; i++)
			k >> matrix[i].f1 >> matrix[i].f2;

    for (t=1; t<N; t++)
        {
            int flag=-2;
            for(i=0; i<t; i++)
            {
                if(n_r(matrix[t],matrix[i]) == true)
                {
                    matrix[t].inPareto = true;
                    matrix[i].inPareto = true;
                    flag=0;
                }
                if(b_r(matrix[t],matrix[i]) == true && b_r(matrix[i],matrix[t]) == false)
                {
                    matrix[t].inPareto = true;
                    matrix[i].inPareto = false;
                    flag=1;
                }
                if(b_r(matrix[i],matrix[t]) == true && b_r(matrix[t],matrix[i]) == false)
                {
                    matrix[i].inPareto = true;
                    matrix[t].inPareto = false;
                    flag=-1;
                }
            }
            if(flag == 1 || flag == 0)
                matrix[t].inPareto = true;
            if(flag == -1 || flag == 0)
                matrix[i].inPareto = true;
        }


    cout << "Решения, вошедшие в Паретто-границу" <<endl;
    for(i=0; i<N; i++)
    {
        if(matrix[i].inPareto == true)
        {
            cout << "x" << i+1 <<" ";
        }
    }
    cout << endl;

    int maxf1 = matrix[0].f1,maxf2 = matrix[0].f2;
    for(i=1; i<N; i++)
    {
        if(matrix[i].f1 > maxf1)
            maxf1=matrix[i].f1;
        if(matrix[i].f2 > maxf2)
            maxf2=matrix[i].f2;
    }

    double minf1f2=999;
    int index;
    for(i=1; i<N; i++)
    {
        if(matrix[i].inPareto == true)
        {
            double temp;
            temp=sqrt((maxf1 -matrix[i].f1)*(maxf1 -matrix[i].f1)+(maxf2 -matrix[i].f2)*(maxf2 -matrix[i].f2));
            if(temp<minf1f2)
            {
                minf1f2=temp;
                index=i;
            }
        }
    }
    cout << "Наиболее предпочтительное решение на Паретто-границе x" << index+1 << endl;
    return 0;
}
