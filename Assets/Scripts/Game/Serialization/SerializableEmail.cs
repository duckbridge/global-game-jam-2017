using UnityEngine;
using System.Collections;

public class SerializableEmail {

    public int emailID;

    public string subject;
    public string text;

    public bool isRead = false;

    public bool Equals(SerializableEmail otherEmail) {
       return subject.Equals(otherEmail.subject) && text.Equals(otherEmail.text);
    }
}
