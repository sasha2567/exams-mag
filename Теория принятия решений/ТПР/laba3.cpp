#include<iostream>
#include<fstream>
#include<iomanip>
#include<string>

using namespace std;

const int N=8; //8
const int M=5; //5

bool b(int x1[M],int x2[M])
{
    if(x1[0] >= x2[0] && x1[1] >= x2[1] && x1[2] >= x2[2] && x1[3] >= x2[3] && x1[4] > x2[4] ||
       x1[0] >= x2[0] && x1[1] >= x2[1] && x1[2] >= x2[2] && x1[3] > x2[3] && x1[4] >= x2[4] ||
       x1[0] >= x2[0] && x1[1] >= x2[1] && x1[2] > x2[2] && x1[3] >= x2[3] && x1[4] >= x2[4] ||
       x1[0] >= x2[0] && x1[1] > x2[1] && x1[2] >= x2[2] && x1[3] >= x2[3] && x1[4] >= x2[4] ||
       x1[0] > x2[0] && x1[1] >= x2[1] && x1[2] >= x2[2] && x1[3] >= x2[3] && x1[4] >= x2[4])
        return true;
    else
        return false;
}

int main() {
	int i, j;
	int K[N][M]={0}; //������� ���������
	int X[N]; //������ �������
	int K1,K2;
	int vector[M];
	int temp;
	for (i = 0; i < N; i++)
		X[i] = 1;
	ifstream k("k.txt", ios::in);
	for (i = 0; i < N; i++)
		for (j = 0; j < M; j++)
			k >> K[i][j];
	//����������� ��������� ����������� �������
	int l, h; //������ �������
	int count = 0;
	for (l = 0; l < N; l++)
		for (h = 0; h < N; h++)
			if (l != h)
				if(b(K[l], K[h]))
                    X[h] = 0;

	cout << "��������� ����������� �������: "<<endl;
	for (i = 0; i < N; i++)
		if(X[i]!=0)
            cout << "x" << i+1 << " ";
	cout << endl;

	//����������� ��������� ����������� ������� �������������� �� ������ ���������� � ������������ ���������
    K1=0, K2=1;
    for(int t=0; t<2; t++)
    {
        for (l = 0; l < N; l++)
        {
            if (X[l] == 1)
            {
                for (j = 0; j < M; j++)
                    vector[j] = K[l][j];
                //������������ ��������� K1 � �2
                temp = vector[K1];
                vector[K1] = vector[K2];
                vector[K2] = temp;
                if(t==0)
                {
                    if (K[l][0] > vector[0]) //���� �(�i) ���������������� �12(�i)
                        for (h = 0; h < N; h++)
                            if (l != h)
                                if(b(vector, K[h]))
                                    X[h] = 0;
                }
                else
                    if(K[l][3]> vector[3])
                        for (h = 0; h < N; h++)
                            if (l != h)
                                if(b(vector, K[h]))
                                    X[h] = 0;
            }
        }
        K1=3, K2=4;
    }

	cout<< "��������� ����������� ������� �������������� �� ������ ��������� ������������: "<<endl;
	for (i = 0; i < N; i++)
		if (X[i] == 1)
			cout << "x" << i+1 << " ";
	cout << endl;
	//����������� ��������� ����������� ������� �������������� �� ������ ���������� �� ��������������� ���������
	K1=1, K2=2;
	for(int t=0; t<2; t++)
    {
        for (l = 0; l < N; l++)
        {
            if (X[l] == 1)
            {
                for (j = 0; j < M; j++)
                    vector[j] = K[l][j];
                //������������ ��������� K1 � �2
                temp = vector[K1];
                vector[K1] = vector[K2];
                vector[K2] = temp;
                for (h = 0; h < N; h++)
                    if (l != h)
                        if(b(vector, K[h]))
                            X[h] = 0;
            }
        }
        K1=2, K2=3;
    }
	cout<< "��������� ����������� ������� �������������� �� ������ ��������� ���������������: "<<endl;
	for (i = 0; i < N; i++)
		if (X[i] == 1)
			cout << "x" << i+1 << " ";
	cout << endl;
	return 0;
}
