package net.cart.action;

import java.io.PrintWriter;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import net.cart.db.CartDAO;
import net.action.Action;
import net.action.ActionForward;

public class CartDetailView implements Action {
	public ActionForward execute(HttpServletRequest request,HttpServletResponse response) throws Exception{
		HttpSession session = request.getSession();
		CartDAO cartdao = new CartDAO();	
		
		int code = Integer.parseInt(request.getParameter("code"));
		
		if(cartdao.detailCart(code) == null) {
			PrintWriter out = response.getWriter();
			out.println("<script>");
			out.println("alert('Register is failed try again')");
			out.print("</script>");
			out.close();
			return null;
		}

		session.setAttribute("detailCart", cartdao.detailCart(code));
		
		ActionForward forward = new ActionForward();
		forward.setRedirect(true);
		forward.setPath("ProductSaleView.po");
		return forward;
		
	}
}


