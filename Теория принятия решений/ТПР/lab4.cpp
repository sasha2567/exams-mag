#include<iostream>
#include<fstream>
#include<iomanip>
#include<string>

using namespace std;

const int N = 4;
const int M = 6;
/*�������� ������� �� ���������������
 � ����� ������� ����������� �������� w*/
float* search_w(float **mtr, int n) {
	int i, j;
	float sum = 0;
	float lamda = 0; //����������� �������� ������� �1
	float index = 0; //������ ���������������
	float *w = new float [n];
	float w1[n];
	float w2[n];
	for (i = 0; i < n; i++) {
		w[i] = 0;
		w1[i] = 0;
		w2[i] = 0;
	}
	//����� ������� ����������� �������� w
	for (i = 0; i < n; i++)
		for (j = 0; j < n; j++) {
			w[i] += mtr[i][j];
			sum += mtr[i][j];
		}
	//������������� �������� �������
	for (i = 0; i < n; i++)
		w[i] = w[i] / sum;
	//�������� ������� �� ���������������
	for (i = 0; i < n; i++) {
		for (j = 0; j < n; j++)
			w1[i] += mtr[i][j] * w[j];
		w2[i] = w1[i] / w[i];
		lamda += w2[i] / n;
	}
	index = (lamda - n) / (n - 1);
	if ((lamda > n) && (n*1.001 < lamda))
		cout << "������� �����������!" << endl;
	else
		cout << "������� �������������!" << endl;
	cout << "������ ����������� ��������:" <<endl;
	for (i = 0; i < n; i++)
		cout << w[i] << " ";
	cout << endl;
	cout << "����������� �������� ������� " <<lamda << endl;
	cout << "������ ��������������� " << index << endl;
	cout << endl;
	return w;
}

int main() {
	int i, j;
	char *subjects[6] = { "������ �������� �������", "������ ����������", "������������������ � �������������� ����������", "������ �������������� ���������", "���������� ��������� ����������", "���������� ����������������"};
	float **A1 = new float*[N]; //������� ������ ��������� �������������
	for (i = 0; i < N; i++)
		A1[i] = new float[N];
	//������� �������
	float **A21 = new float*[M];
	for (i = 0; i < M; i++)
		A21[i] = new float[M];
	float **A22 = new float*[M];
	for (i = 0; i < M; i++)
		A22[i] = new float[M];
	float **A23 = new float*[M];
	for (i = 0; i < M; i++)
		A23[i] = new float[M];
	float **A24 = new float*[M];
	for (i = 0; i < M; i++)
		A24[i] = new float[M];
	//������� ����������� �������� ������
	float *wA1 = new float [N];
	float *wA21 = new float [M];
	float *wA22 = new float [M];
	float *wA23 = new float [M];
	float *wA24 = new float [M];
	float w[N][M];
	//������ ������� ������������� w2i
	float W[M][N]={0};
	float D[M]={0}; //������ �������
	ifstream a1("mtr.txt", ios::in);
	ifstream a21("1.txt", ios::in);
	ifstream a22("2.txt", ios::in);
	ifstream a23("3.txt", ios::in);
	ifstream a24("4.txt", ios::in);
	cout.precision(3);
	for (i = 0; i < N; i++)
		for (j = 0; j < N; j++)
			a1 >> A1[i][j];
	for (i = 0; i < M; i++)
		for (j = 0; j < M; j++) {
			a21 >> A21[i][j];
			a22 >> A22[i][j];
			a23 >> A23[i][j];
			a24 >> A24[i][j];
		}
	a1.close();
	a21.close();
	a22.close();
	a23.close();
	a24.close();
	cout << "������� ������ ��������� �������������:" << endl;
	for (i = 0; i < N; i++) {
		for (j = 0; j < N; j++)
			cout << setw(5) << A1[i][j] << " ";
		cout << endl;
	}
	wA1 = search_w(A1, N);
	cout << "������� ������ ��������� ������� A21:" << endl;
	for (i = 0; i < M; i++) {
		for (j = 0; j < M; j++)
			cout << setw(5) << A21[i][j] << " ";
		cout << endl;
	}
	wA21 = search_w(A21, M);
	cout << "������� ������ ��������� ������� A22:" << endl;
	for (i = 0; i < M; i++) {
		for (j = 0; j < M; j++)
			cout << setw(5) << A22[i][j] << " ";
		cout << endl;
	}
	wA22 = search_w(A22, M);
	cout << "������� ������ ��������� ������� A23:" << endl;
	for (i = 0; i < M; i++) {
		for (j = 0; j < M; j++)
			cout << setw(5) << A23[i][j] << " ";
		cout << endl;
	}
	wA23 = search_w(A23, M);
	cout << "������� ������ ��������� ������� A24:" << endl;
	for (i = 0; i < M; i++) {
		for (j = 0; j < M; j++)
			cout << setw(5) << A24[i][j] << " ";
		cout << endl;
	}
	wA24 = search_w(A24, M);
	//������������ �������� W
	for (i = 0; i < M; i++) {
		w[0][i] = wA21[i];
		w[1][i] = wA22[i];
		w[2][i] = wA23[i];
		w[3][i] = wA24[i];
	}
	for (i = 0; i < N; i++)
		for (j = 0; j < M; j++)
			W[j][i] = w[i][j];
	cout << "������� ������� ������������� w2i" << endl;
	for (i = 0; i < M; i++) {
		cout << "W2" << i+1 << "  ";
		for (j = 0; j < N; j++)
			cout << W[i][j] << " ";
		cout<<endl;
	}
	//������ ������ ������� D
	for (i = 0; i < M; i++)
		for (j = 0; j < N; j++)
			D[i] += W[i][j] * wA1[j];
	cout << endl << "������ �������" << endl;
	float max = D[0];
	int maxi = 0;
	//����� ������ � ����� ����������������� �������
	for (i = 0; i < M; i++) {
		cout << "D" << i+1 <<" = " << D[i] << " ";
		if (D[i] > max) {
			max = D[i];
			maxi = i;
		}
	}
	cout << endl <<endl << "������� �� ������ '" << subjects[maxi] << "'" << endl;;
	return 0;
}
