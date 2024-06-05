Public Class Rain

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rgn">rgn mast start from 1 index, first = 0</param>
    ''' <param name="my_n"></param>
    ''' <param name="sig_02_div_Ktg"></param>
    ''' <returns></returns>
    Function Rain_Flow(rgn As List(Of Double), my_n As Double, sig_02_div_Ktg As Double) As Double
        Dim k0 As Integer, k_tol As Integer, tol As Double
        Dim i As Integer, j As Integer, k As Integer
        Dim m As Integer, m_cur As Integer, n As Integer
        Dim kMaxVal As Integer, s As Integer
        Dim k_log As Boolean

        k0 = rgn.Count - 1
        k_tol = 8

        Dim sig_eq As Double, xa As Double, xm As Double, sig As Double

        Dim buff(k0 + 1)
        Dim xbuff(k0 + 1)
        Dim x_cyc(k0 + 1, 2)

        'ReDim buff(k0 + 1)
        'ReDim xbuff(k0 + 1)
        'ReDim x_cyc(k0 + 1, 2)

        For i = 1 To k0
            xbuff(i) = Math.Round(rgn(i), k_tol)
        Next
        xbuff(k0 + 1) = xbuff(1)


        If k0 < 3 Then
            sig_eq = 0
            GoTo stp10
        End If

        Dim maxVal = xbuff(1)
        kMaxVal = 1
        s = 0
        For i = 1 To k0
            If xbuff(k0 - i + 2) <> xbuff(k0 - i + 1) Then
                k_log = xbuff(k0 - i + 2) > xbuff(k0 - i + 1)
                s = 1
                Exit For
            End If
        Next

        If s = 0 Then
            sig_eq = 0
            GoTo stp10
        End If

        j = 0
        For i = 1 To k0
            If k_log And xbuff(i + 1) < xbuff(i) Then
                j = j + 1
                k_log = Not k_log
                buff(j) = xbuff(i)

                If buff(j) >= maxVal Then
                    maxVal = buff(j)
                    kMaxVal = j
                End If

            ElseIf Not k_log And xbuff(i + 1) > xbuff(i) Then
                j = j + 1
                k_log = Not k_log
                buff(j) = xbuff(i)
            End If
        Next


        m = 0
        For i = kMaxVal To j
            m = m + 1
            xbuff(m) = buff(i)
        Next

        For i = 1 To kMaxVal - 1
            m = m + 1
            xbuff(m) = buff(i)
        Next
        xbuff(m + 1) = xbuff(1)

        k = 0
        m_cur = 0
        n = 0
stp1:
        n = n + 1
        m_cur = m_cur + 1
        buff(n) = xbuff(m_cur)
stp2:
        If n < 3 Then GoTo stp1
        Dim x2_cur = Math.Abs(buff(n) - buff(n - 1))
        Dim x1_cur = Math.Abs(buff(n - 1) - buff(n - 2))
stp3:
        If x2_cur < x1_cur Then GoTo stp1
stp4:
        k = k + 1
        x_cyc(k, 1) = buff(n - 2)
        x_cyc(k, 2) = buff(n - 1)
        n = n - 2
        buff(n) = buff(n + 2)
        If n = 1 Then xbuff(m + 1) = buff(1)
        If m_cur = m + 1 And n = 1 Then GoTo stp5
        GoTo stp2
stp5:

        sig_eq = 0#
        For i = 1 To k
            xa = Math.Abs((x_cyc(i, 1) - x_cyc(i, 2))) / 2.0#
            xm = (x_cyc(i, 1) + x_cyc(i, 2)) / 2.0#
            Dim xmax = Math.Max(x_cyc(i, 1), x_cyc(i, 2))
            Dim xmin = Math.Max(x_cyc(i, 1), x_cyc(i, 2))


            If xm >= 0 Then
                sig = Math.Sqrt(2 * xmax * xa)

            ElseIf xmax > 0 And Math.Abs(xmin) < sig_02_div_Ktg Then
                sig = Math.Sqrt(2.0#) * (xa + 0.2 * xm)

            ElseIf xmax > 0 And Math.Abs(xmin) >= sig_02_div_Ktg Then
                sig = Math.Sqrt(2.0#) / 2 * (xmax + sig_02_div_Ktg + 0.2 * (xmax - sig_02_div_Ktg))
            Else
                sig = 0
            End If
            sig_eq = sig_eq + sig ^ my_n
        Next
        sig_eq = sig_eq ^ (1.0# / my_n)
stp10:
        Rain_Flow = sig_eq
    End Function

End Class
