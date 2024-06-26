using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageCtrl : MonoBehaviour
{
    public Text PlayB, MusicT, TutorialB, QuitL,EasyModeB;
    public Text T,T1,T2,T3,T4,T5;
    public MainMenu MM;
    public void ToRus()
    {
        PlayB.text = "������";
        MusicT.text = "������";
        TutorialB.text = "�������- ����������";
        QuitL.text = "�����: Alt+F4";
        EasyModeB.text = "˸���� �����";
        T.text = "������� ������ ������ ��������, ����� ��� ����� ����� ��� �� ����� ���� �������� �������, ��������� ��� ����������� �������\nWASD(����).";
        T1.text = "������ ����������������� � �����, ��� ��������� ������� � �� ������, � � �����, � ������� �������\nSpace(������).";
        T2.text = "������� ������ ���� � ������� ������ ������ � ��, ��� ���� � �������. �� ���������� ���� ���������, ��� �������� ���������� ������ ���� �� ���� ������, ��� ��� ��� ���� �� ������� �������� �� ��������� �������.";
        T3.text = "������ ���� � ������������, ��� ����-������� ����������� ����� ������ ������ �� ����������. ����� �������� ��� ���� ����, �� ����������� �������� �� ������������ ������������� ����������, ���� ������ ����, ��� ��������.";
        T4.text = "����� ����� ������ ��������, ������� ��� ���� ��������� ������� ����� ������ ���������� �����, �� ������ ���� ����������� ����� ����� ������ ��������� ���, ���� ������, �������� ���������� ���������.";
        T5.text = "�������� �� �� ���, ������ �������� ����� � �� ������ ������, ����� ��������� �������, ������ ���-�� �������� � ��� � ����� ������� �������. ������� ���� ���� ������������ �� ��? ���� �� ����� ���� ������������ ��������, �����, ����� �� ��������� �� ��������� �����.";
        MM.isEng = false;
    }
    public void ToEng()
    {
        PlayB.text = "Play";
        MusicT.text = "Music";
        TutorialB.text = "Story-\ntutorial";
        QuitL.text = "To quit: Alt+F4";
        EasyModeB.text = "Easy mode";
        T.text = "Little Match was severely punished that without a bag of bits, she would not even dare to leave the square, using the WASD keys to move.";
        T1.text = "Match interacted with the world, where it is equally cold on the streets and in the souls, using the Space key.";
        T2.text = "A big green scarf and a handful of sulfur matches were all the baby had. She could only hope that passersby would agree to buy at least one match, or that they would at least pay attention to the little filly.";
        T3.text = "There was nothing to expect that Match would be able to earn more or less the necessary amount of money. To survive another night, she had to give everything she earned to the hard-hearted stallkeepers if there was anything to give at all.";
        T4.text = "At night, the little one had nowhere to go, so she was forced to huddle around a rusty, sooty barrel, keeping the hearth warm with a miserable firewood or, if lucky, a stingy stallkeeper's coal until the morning.";
        T5.text = "Despite everything, Match found peace in those rare moments when she managed to sleep, as if someone was protecting her dream and giving her smoldering hope. Was there really anyone to worry about her? If only the poor thing had lasted six days, maybe then she would have been allowed to return home.";
        MM.isEng = true;
    }
}
